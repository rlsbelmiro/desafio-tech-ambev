import { canSSRAuth } from "../../utils/canSSRAuth";
import Head from 'next/head';
import Header from "../../components/Header";
import styles from './styles.module.scss';

export default function Dashboard() {
    return (
        <>
            <Head>
                <title>Painel - DevAmbev</title>
            </Head>
            <div className={styles.container}>
                <Header />
                <h2>Bem vindo, escolha uma opção do menu acima para iniciar</h2>
            </div>
        </>
        
    )
}

export const getServerSideProps = canSSRAuth(async (ctx) => {
    return  {
      props: {}
    }
});