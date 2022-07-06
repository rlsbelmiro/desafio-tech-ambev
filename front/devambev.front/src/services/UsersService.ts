import { setupAPIClient } from "./app";

export const api = setupAPIClient();

export interface UserModel {
    id: number,
    name: string,
    email: string,
    password: string,
    active: boolean,
}

export interface BaseResponse {
    message: string,
    success: boolean
}

export async function listUsers():  Promise<UserModel[]> {
    let result = [] as UserModel[];
    try {
        const response = await api.get('/User');
        const { success, message, users } = response.data;
        if(success) {
            result = users;
        }
    } catch (err) {

    }
    return result;
}

export async function getUser(id: number): Promise<UserModel> {
    let result = {} as UserModel;
    try {
        const response = await api.get(`/User/${id}`);
        const { success, message, name, email, active } = response.data;
        if(success) {
            result = {
                id,
                name,
                email,
                active,
                password: ''
            }
        }
    } catch (err) {

    }
    return result;
}

export async function saveUser({id, name, password, email, active}: UserModel): Promise<BaseResponse> {
    if(id > 0) {
        const response = await api.put(`/User/${id}`, {
            name,
            email,
            document,
            active
        });
        const { success, message } = response.data;
        return {
            success,
            message
        };
    } else {
        const response = await api.post(`/User`, {
            name,
            password,
            email,
            active
        });
        const { success, message } = response.data;
        return {
            success,
            message
        };
    }
}

export async function deleteUser(id: number): Promise<BaseResponse> {
    try {
        const response = await api.delete(`/User/${id}`);
        const { success, message } = response.data;
        return {
            success,
            message
        }
    } catch(err) {
        return {
            success: false,
            message: err.response.data
        }
    }
}