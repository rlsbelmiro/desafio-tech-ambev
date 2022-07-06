import styles from './styles.module.scss';
import { ButtonHTMLAttributes, ReactNode } from 'react';
import { FaSpinner } from 'react-icons/fa'

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement>{
    loading?: boolean,
    children: ReactNode
}

export function Button({loading, children, ...rest}: ButtonProps) {
    return (
        <button disabled={loading} className={styles.button}>
            {loading ? <FaSpinner color="#fff" /> : (
                <a className={styles.buttonText}>
                    {children}
                </a>
            )}
            
        </button>
    )
}