import Link from 'next/link';
import styles from './styles.module.scss';
import { FiLogOut } from 'react-icons/fi';
import logoImg from '../../../public/logo.svg';
import { useContext } from 'react';
import { AuthContext } from '../../contexts/AuthContext';

interface HeaderProps {
    active?: boolean;
}

export default function Header({ active }: HeaderProps) {
    const { signOut } = useContext(AuthContext);
    return (
        <header className={styles.headerContainer}>
            <div className={styles.headerContent}>
                <Link href="/dashboard">
                    <h1 className={styles.logo}>DevAmbev - Tech</h1>
                </Link>
                <nav className={styles.menuNav}>
                    <Link href="orders">
                        <a>Pedidos</a>
                    </Link>
                    <Link href="users">
                        <a>Usuários</a>
                    </Link>
                    <Link href="customers">
                        <a>Clientes</a>
                    </Link>

                    <Link href="products">
                        <a>Produtos</a>
                    </Link>

                    <button onClick={() => signOut()}>
                        <FiLogOut color="#fff" size={24} />
                    </button>
                </nav>
            </div>
        </header>
    )
}