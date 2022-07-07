import { canSSRAuth } from "../../utils/canSSRAuth";
import Head from 'next/head';
import Header from "../../components/Header";
import styles from './styles.module.scss';
import { ChangeEvent, FormEvent, useEffect, useState } from "react";
import { FiUpload } from "react-icons/fi";
import { BiEdit, BiTrash } from "react-icons/bi";
import { toast } from "react-toastify";
import { api } from "../../services/apiClient";
import { Input } from "../../components/ui/Input/index";
import { Button } from "../../components/ui/Button/index";
import { CustomerModel, deleteCustomer, getCustomer, listCustomers, saveCustomer } from "../../services/CustomerService";

export default function Customer() {
    const [idCustomer, setIdCustomer] = useState(0);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [document, setDocument] = useState('');
    const [active, setActive] = useState(true);
    const [loading, setLoading] = useState(false);
    const [customers, setCustomers] = useState([] as CustomerModel[]);

    async function load() {
        try {
            const data = await listCustomers();
            setCustomers(data);
        } catch(err) {

        }
    }

    async function handleSave(e: FormEvent) {
        e.preventDefault();
        setLoading(true);
        try {
            let withErrors = false;
            if(!name) {
                withErrors = true;
                toast.warning('Informe o nome do cliente')
            }
            if(!email) {
                withErrors = true;
                toast.warning('Informe o email do cliente')
            }
            if(!document) {
                withErrors = true;
                toast.warning('Informe a número de documento do cliente')
            }
            
            if(withErrors) {
                return;
            } else {
                const response = await saveCustomer({
                    id: idCustomer,
                    name,
                    document,
                    email,
                    active
                });
                
                if(response.success) {
                    toast.success(response.message);
                    setName('');
                    setDocument('');
                    setEmail('');
                    setActive(true);
                    setIdCustomer(0);
                    await load();
                } else {
                    toast.error(response.message);
                }
            }
        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao cadastrar cliente!'
            toast.error(msg);
        } finally {
            setLoading(false);
        }
    }

    async function handleEdit(id: number) {
        if(id > 0) {
            const data = await getCustomer(id);
            setIdCustomer(id);
            setName(data.name);
            setEmail(data.email);
            setDocument(data.document);
            setActive(data.active);
        }
    }

    async function handleDelete(id: number) {
        if(id > 0) {
            const response = await deleteCustomer(id);
            const { success, message } = response;
            if(success) {
                toast.success(message);
                await load();
            } else {
                toast.error(message);
            }
        }
    }

    useEffect(() => {
        load();
    },[]);

    return (
        <>
            <Head>
                <title>Cadastro de clientes</title>
            </Head>
            <Header />
            <main className={styles.container}>
                <h1>Cadastrar cliente</h1>
                <form onSubmit={(e) => handleSave(e)} className={styles.form}>
                    <Input type="text" placeholder="Digite o nome do cliente"
                        className={styles.input}
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <Input type="text" placeholder="Digite o email do cliente"
                        className={styles.input}
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <Input type="text" placeholder="Digite o número de documento do cliente"
                        className={styles.input}
                        value={document}
                        onChange={(e) => setDocument(e.target.value)}
                    />
                    
                    <div className={styles.containerCheckbox}>
                        <Input type="checkbox" checked={active}
                            onChange={(e) => setActive(e.target.checked)}
                        /> Cadastro ativo
                    </div>
                    <Button loading={loading}>
                        Cadastrar
                    </Button>
                </form>
                <h1>Lista de clientes</h1>
                <table className={styles.table}>
                    <thead>
                    <tr>
                        <td>Id</td>
                        <td>Nome</td>
                        <td>Email</td>
                        <td>Ativo</td>
                        <td>Ações</td>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        customers.map(p => {
                            return (
                                <tr key={p.id}>
                                    <td>{p.id}</td>
                                    <td>{p.name}</td>
                                    <td>{p.email}</td>
                                    <td>{p.active ? 'Sim' : 'Não'}</td>
                                    <td>
                                        <a onClick={() => handleEdit(p.id)}>
                                            <BiEdit />
                                        </a>
                                        <a onClick={() => handleDelete(p.id)}>
                                            <BiTrash />
                                        </a>
                                    </td>
                                </tr>
                            )
                        })
                    }
                    </tbody>
                </table>
            </main>
        </>
        
    )
}

export const getServerSideProps = canSSRAuth(async (ctx) => {
    return  {
      props: {}
    }
});