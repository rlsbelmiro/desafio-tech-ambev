import { canSSRAuth } from "../../utils/canSSRAuth";
import Head from 'next/head';
import Header from "../../components/Header";
import styles from './styles.module.scss';
import { ChangeEvent, FormEvent, useEffect, useState } from "react";
import { FiUpload } from "react-icons/fi";
import { BiEdit, BiTrash, BiListPlus, BiZoomIn } from "react-icons/bi";
import { toast } from "react-toastify";
import { api } from "../../services/apiClient";
import { Input } from "../../components/ui/Input/index";
import { Button } from "../../components/ui/Button/index";
import { getOrder, listOrders, OrderItemModel, OrderModel, saveOrder } from "../../services/OrdersService";
import { CustomerModel, listCustomers } from "../../services/CustomerService";
import { listProducts, ProductModel } from "../../services/productsService";

export default function Order() {
    const [idOrder, setIdOrder] = useState(0);
    const [customerId, setCustomerId] = useState(0);
    const [loading, setLoading] = useState(false);
    const [orders, setOrders] = useState([] as OrderModel[]);
    const [items, setItems] = useState([] as OrderItemModel[]);
    const [customers, setCustomers] = useState([] as CustomerModel[]);
    const [products, setProducts] = useState([] as ProductModel[]);
    const [quantity, setQuantity] = useState('');
    const [productId, setProductId] = useState(0);

    async function load() {
        try {
            const data = await listOrders();
            setOrders(data || [] as OrderModel[]);
            const dataCustomer = await listCustomers();
            setCustomers(dataCustomer);
            const dataProducts = await listProducts();
            setProducts(dataProducts);
        } catch(err) {

        }
    }

    async function handleSave(e: FormEvent) {
        e.preventDefault();
        setLoading(true);
        try {
            let withErrors = false;
            if(customerId <= 0) {
                withErrors = true;
                toast.warning('Informe o cliente do pedido')
            }
            if(items.length == 0) {
                withErrors = true;
                toast.warning('Selecione ao menos um produto');
            }
            
            if(withErrors) {
                return;
            } else {
                const response = await saveOrder({
                    id: idOrder,
                    customerId,
                    items
                });
                
                if(response.success) {
                    toast.success(response.message);
                    setCustomerId(0);
                    setItems([]);
                    setIdOrder(0);
                    await load();
                } else {
                    toast.error(response.message);
                }
            }
        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao cadastrar pedido!'
            toast.error(msg);
        } finally {
            setLoading(false);
        }
    }

    async function handleEdit(id: number) {
        if(id > 0) {
            const data = await getOrder(id);
            setIdOrder(id);
            setCustomerId(data.customerId);
            setItems(data.items || []);
        }
    }

    function reset(){
        setCustomerId(0);
        setItems([]);
        setIdOrder(0);
        setProductId(0);
        setQuantity('');
    }

    function addProduct() {
        if(productId > 0) {
            if(!quantity || quantity == '0') {
                toast.warning('Informe a quantidade');
                return;
            }
            const prd = products.filter(x => x.id == productId);
            const prdExist = items.filter(x => x.productId == productId);
            if(prdExist && prdExist.length > 0) {
                toast.warning('Produto já foi adicionado');
                return;
            }
            let arr = items;
            if(prd && prd.length > 0) {
                arr.push({
                    quantity: parseInt(quantity),
                    productId: prd[0].id,
                    productName: prd[0].name,
                    price: prd[0].price.toFixed(2)
                });
                setItems(arr);
                setProductId(0);
                setQuantity('');
            }
        } else {
            toast.warning('Selecione o produto')
        }
    }

    function removeProduct(id: number) {
        let arr = [];
        items.forEach(i => {
            if(i.productId != id) {
                arr.push(i);
            }
        });
        setItems(arr);
    }

    useEffect(() => {
        load();
    },[]);

    return (
        <>
            <Head>
                <title>Cadastro de pedido</title>
            </Head>
            <Header />
            <main className={styles.container}>
                <h1>Cadastrar pedidos</h1>
                <form onSubmit={(e) => handleSave(e)} className={styles.form}>
                    <select value={customerId} onChange={(e) => setCustomerId(parseInt(e.target.value))}>
                        <option>Selecione o cliente</option>
                        {
                            customers.map(c => {
                                return (
                                    <option key={c.id} value={c.id}>{c.name}</option>
                                )
                            })
                        }
                    </select>
                    <h3>Produtos</h3>
                    <div className={styles.products}>
                        <select value={productId} onChange={(e) => setProductId(parseInt(e.target.value))}>
                            <option>Selecione o produto</option>
                            {
                                products.map(p => {
                                    return (
                                        <option key={p.id} value={p.id}>{`${p.name} - R$ ${p.price.toFixed(2)}`}</option>
                                    )
                                })
                            }
                        </select>
                        <Input placeholder="Quantidade" 
                            value={quantity}
                            onChange={(e) => setQuantity(e.target.value)}
                        />
                        <a className={styles.botaoAdd} onClick={() => addProduct()}>
                            <BiListPlus />
                        </a>
                        
                    </div>
                    <table className={styles.table}>
                            <thead>
                                <tr>
                                    <td>Produto</td>
                                    <td>Quantidade</td>
                                    <td>Preço</td>
                                    <td>Ações</td>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    items.map(i => {
                                        return (
                                            <tr key={i.productId}>
                                                <td>{i.productName}</td>
                                                <td>{i.quantity}</td>
                                                <td>{i.price}</td>
                                                <td>
                                                    <a onClick={() => removeProduct(i.productId)}>
                                                        {
                                                            idOrder == 0 && (
                                                                <BiTrash />
                                                            )
                                                        }
                                                    </a>
                                                </td>
                                            </tr>
                                        )
                                    })
                                }
                            </tbody>
                        </table>
                    {
                        idOrder == 0 && (
                            <Button loading={loading}>
                                Cadastrar
                            </Button>
                        )
                    }
                    <a onClick={() => reset()} className={styles.buttonAdd}>Limpar formulário</a>
                </form>
                <h1>Lista de usuários</h1>
                <table className={styles.table}>
                    <thead>
                    <tr>
                        <td>Id</td>
                        <td>Valor</td>
                        <td>Cliente</td>
                        <td>Ações</td>
                    </tr>
                    </thead>
                    <tbody>
                    {
                        orders.map(p => {
                            return (
                                <tr key={p.id}>
                                    <td>{p.id}</td>
                                    <td>{p.amount.toFixed(2)}</td>
                                    <td>{p.customerName}</td>
                                    <td>
                                        <a onClick={() => handleEdit(p.id)}>
                                            <BiZoomIn />
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