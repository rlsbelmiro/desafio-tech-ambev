import Router from "next/router";
import { destroyCookie, parseCookies, setCookie } from "nookies";
import { createContext, ReactNode, useEffect, useState } from "react";
import { toast } from "react-toastify";
import { api } from "../services/apiClient";

type AuthContextData = {
    user: UserProps;
    isAuthenticated: boolean;
    signIn: (credencials: SignInProps) => Promise<void>;
    signOut: () => void;
    signUp: (credecials: SignUpProps) => Promise<UserProps>;
}

type UserProps = {
    id: string;
    name: string;
    email: string;
}

type SignInProps = {
    email: string;
    password: string;
}

type SignUpProps = {
    name: string;
    email: string;
    password: string;
}

type AuthProviderProps = {
    children: ReactNode;
}

export const AuthContext = createContext({} as AuthContextData);

export function signOut() {
    try {
        destroyCookie(undefined,'@nextauth.token');
        Router.push('/');
    } catch {
        console.log('Erro ao sair do sistema');
    }
}

export function AuthProvider({ children }: AuthProviderProps) {
    const [user, setUser] = useState<UserProps>();
    const isAuthenticated = !!user;

    useEffect(() => {
        const { '@nextauth.token' : token } = parseCookies(undefined);
        if(token) {
            api.get('/me').then(res => {
                if(res.data) {
                    const { id, name, email } = res.data;
                    setUser({
                        id,
                        name,
                        email
                    });
                } else {
                    signOut();
                }
            });
        } else {
            signOut();
        }
    },[]);

    async function signIn({email, password}: SignInProps) {
        try {
            const response = await api.post('/Login', {
                email,
                password
            });
            const { id, name, token, success, message } = response.data;
            if(success) {
                setUser({
                    id,
                    name,
                    email
                });
                setCookie(undefined, '@nextauth.token', token, {
                    maxAge: 60 * 60 * 24 * 30,
                    path: '/'
                });
                api.defaults.headers['Authorization'] = `Bearer ${token}`;
                Router.push('/dashboard');
            } else {
                toast.error(message)
            }
        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao acessar!'
            toast.error(msg);
        }
    }

    async function signUp({name, email, password}: SignUpProps) {
        try {
            const response = await api.post('/User', {
                name,
                email,
                password
            });

            const { success, message } = response.data;

            if(success)
            {
                Router.push('/');
                toast.success('Conta criada com sucesso!');
                return response.data;
            } 
            else {
                toast.error(message);
            }

        } catch (err) {
            const msg = err?.response?.data ? err.response.data : 'Erro ao acessar!'
            toast.error(msg);
        }
    }
    return (
        <AuthContext.Provider value={{ user, isAuthenticated, signIn, signOut, signUp }}>
            {children}
        </AuthContext.Provider>
    )
}