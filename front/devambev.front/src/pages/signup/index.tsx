import Head from 'next/head';

import styles from '../../../styles/home.module.scss';
import { Input } from '../../components/ui/Input';
import { Button } from '../../components/ui/Button';
import Link from 'next/link';
import { FormEvent, useContext, useState } from 'react';
import { AuthContext } from '../../contexts/AuthContext';
import { toast } from 'react-toastify';
import { canSSRGuest } from '../../utils/canSSRGuest';

export default function SignUp() {
  const { signUp } = useContext(AuthContext);

  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);

  async function handleSignUp(e: FormEvent) {
    e.preventDefault();
    if(name === '' || email === '' || password === '') {
      toast.warning('Preencha os dados');
      return;
    }

    setLoading(true);
    let user = {
      name,
      email,
      password
    }
    await signUp(user);
    setLoading(false);
  }
  
  return (
    <>
      <Head>
        <title>Faça seu cadastro</title>
      </Head>
      <div className={styles.containerCenter}>
      <h1 className={styles.logo}>DevAmbev - Tech</h1>
        <div className={styles.login}>
          <h1>Criando sua conta</h1>
          <form onSubmit={(e) => handleSignUp(e)}>
          <Input placeholder='Informe seu nome' value={name} onChange={(e) => setName(e.target.value)} />
            <Input placeholder='Digite seu email' value={email} onChange={(e) => setEmail(e.target.value)} />
            <Input placeholder='Sua senha' type="password" value={password} onChange={(e) => setPassword(e.target.value) } />
            <Button type="button" loading={loading}>
              Cadastrar
            </Button>
            <Link href="/">
              <a className={styles.text}>
                Já possui uma conta? Faça o login
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
