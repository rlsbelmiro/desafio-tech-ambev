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
import { deleteProduct, getProduct, listProducts, ProductModel, saveProduct } from "../../services/ProductsService";

export default function Product() {
    const [idProduct, setIdProduct] = useState(0);
    const [name, setName] = useState('');
    const [price, setPrice] = useState('');
    const [description, setDescription] = useState('');
    const [quantity, setQuantity] = useState('');
    const [active, setActive] = useState(true);
    const [loading, setLoading] = useState(false);
    const [products, setProducts] = useState([] as ProductModel[]);

    async function load() {
        try {
            const data = await listProducts();
            setProducts(data);
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
                toast.warning('Informe o nome do produto')
            }
            if(!price || price == '0') {
                withErrors = true;
                toast.warning('Informe o preço do produto')
            }
            if(!description) {
                withErrors = true;
                toast.warning('Informe a descrição do produto')
            }
            if(!quantity) {
                withErrors = true;
                toast.warning('Informe a quantidade disponível')
            }
            if(withErrors) {
                return;
            } else {
                const response = await saveProduct({
                    id: idProduct,
                    name: name,
                    description: description,
                    price: parseFloat(price),
                    active: active,
                    quantity: parseInt(quantity)
                });
                
                if(response.success) {
                    toast.success(response.message);
                    setName('');
                    setPrice('');
                    setDescription('');
                    setQuantity('');
                    setActive(true);
                    setIdProduct(0);
                    await load();
                } else {
                    toast.error(response.message);
                }
            }
        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao cadastrar produto!'
            toast.error(msg);
        } finally {
            setLoading(false);
        }
    }

    async function handleEdit(id: number) {
        if(id > 0) {
            const data = await getProduct(id);
            setName(data.name);
            setDescription(data.description);
            setPrice(data.price.toFixed(2));
            setActive(data.active);
            setQuantity(data.quantity.toString());
            setIdProduct(id);
        }
    }

    async function handleDelete(id: number) {
        if(id > 0) {
            const response = await deleteProduct(id);
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
                <title>Cadastro de produtos</title>
            </Head>
            <Header />
            <main className={styles.container}>
                <h1>Cadastrar produto</h1>
                <form onSubmit={(e) => handleSave(e)} className={styles.form}>
                    <Input type="text" placeholder="Digite o nome do produto"
                        className={styles.input}
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <Input type="text" placeholder="Digite o preço do produto"
                        className={styles.input}
                        value={price}
                        onChange={(e) => setPrice(e.target.value)}
                    />
                    <Input type="text" placeholder="Digite o quantidade"
                        className={styles.input}
                        value={quantity}
                        onChange={(e) => setQuantity(e.target.value)}
                    />
                    
                    <Input placeholder="Digite a descrição do produto"
                        className={styles.input}
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
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
                <h1>Lista de produtos</h1>
                <table className={styles.table}>
                    <thead>
                    <tr>
                        <td>Id</td>
                        <td>Nome</td>
                        <td>Preço</td>
                        <td>Ativo</td>
                        <td>Ações</td>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        products.map(p => {
                            return (
                                <tr key={p.id}>
                                    <td>{p.id}</td>
                                    <td>{p.name}</td>
                                    <td>{p.price}</td>
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