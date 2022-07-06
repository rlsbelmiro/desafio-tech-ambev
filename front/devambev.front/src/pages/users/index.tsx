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
import { deleteUser, getUser, listUsers, saveUser, UserModel } from "../../services/UsersService";

export default function User() {
    const [idUser, setIdUser] = useState(0);
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [active, setActive] = useState(true);
    const [loading, setLoading] = useState(false);
    const [users, setUsers] = useState([] as UserModel[]);

    async function load() {
        try {
            const data = await listUsers();
            
            setUsers(data || [] as UserModel[]);
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
                toast.warning('Informe o nome do usuário')
            }
            if(!email) {
                withErrors = true;
                toast.warning('Informe o email do usuário')
            }
            if(!password) {
                withErrors = true;
                toast.warning('Informe a senha do usuário')
            }
            
            if(withErrors) {
                return;
            } else {
                const response = await saveUser({
                    id: idUser,
                    name,
                    password,
                    email,
                    active
                });
                
                if(response.success) {
                    toast.success(response.message);
                    setName('');
                    setPassword('');
                    setEmail('');
                    setActive(true);
                    setIdUser(0);
                    await load();
                } else {
                    toast.error(response.message);
                }
            }
        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao cadastrar usuário!'
            toast.error(msg);
        } finally {
            setLoading(false);
        }
    }

    async function handleEdit(id: number) {
        if(id > 0) {
            const data = await getUser(id);
            setIdUser(id);
            setName(data.name);
            setEmail(data.email);
            setActive(data.active);
        }
    }

    async function handleDelete(id: number) {
        if(id > 0) {
            const response = await deleteUser(id);
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
                <title>Cadastro de usuário</title>
            </Head>
            <Header />
            <main className={styles.container}>
                <h1>Cadastrar usuário</h1>
                <form onSubmit={(e) => handleSave(e)} className={styles.form}>
                    <Input type="text" placeholder="Digite o nome do cliente"
                        className={styles.input}
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <Input type="text" placeholder="Digite o email do produto"
                        className={styles.input}
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    {
                        idUser == 0 && (
                            <Input type="password" placeholder="Digite o senha do usuário"
                                className={styles.input}
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        )
                    }
                    
                    <div className={styles.containerCheckbox}>
                        <Input type="checkbox" checked={active}
                            onChange={(e) => setActive(e.target.checked)}
                        /> Cadastro ativo
                    </div>
                    <Button loading={loading}>
                        Cadastrar
                    </Button>
                </form>
                <h1>Lista de usuários</h1>
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
                        users.map(p => {
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