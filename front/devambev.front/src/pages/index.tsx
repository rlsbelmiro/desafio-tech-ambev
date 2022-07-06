import Head from 'next/head';
import styles from '../../styles/home.module.scss';
import { Input } from '../components/ui/Input';
import { Button } from '../components/ui/Button';
import Link from 'next/link';

import { AuthContext } from '../contexts/AuthContext';
import { FormEvent, useContext, useState } from 'react';
import { toast } from 'react-toastify';
import { canSSRGuest } from '../utils/canSSRGuest';

export default function Home() {
  const { signIn } = useContext(AuthContext);
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  async function handleLogin(e: FormEvent) {
    e.preventDefault();
    if(email === '' || password === '') {
      toast.warning('Preencha os dados');
      return;
    }
    setLoading(true);
    await signIn({
      email,
      password
    });
    setLoading(false);
  }
  return (
    <>
      <Head>
        <title>Pizzaria</title>
      </Head>
      <div className={styles.containerCenter}>
        <h1 className={styles.logo}>DevAmbev - Tech</h1>
        <div className={styles.login}>
          <form onSubmit={handleLogin}>
            <Input placeholder='Digite seu email' type="text" value={email} onChange={(e) => setEmail(e.target.value)} />
            <Input placeholder='Sua senha' type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            <Button type="button" loading={loading}>
              Acessar
            </Button>
            <Link href="/signup">
              <a className={styles.text}>
                Não possui uma conta? Cadastre-se
              </a>
            </Link>
          </form>
        </div>
      </div>
    </>
  )
}

export const getServerSideProps = canSSRGuest(async (ctx) => {
  return  {
    props: {}
  }
});