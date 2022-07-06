import { setupAPIClient } from "./app";

export const api = setupAPIClient();

export interface CustomerModel {
    id: number,
    name: string,
    document: string,
    email: string,
    active: boolean,
}

export interface BaseResponse {
    message: string,
    success: boolean
}

export async function listCustomers():  Promise<CustomerModel[]> {
    let result = [] as CustomerModel[];
    try {
        const response = await api.get('/Customer');
        const { success, message, customers } = response.data;
        if(success) {
            result = customers;
        }
    } catch (err) {

    }
    return result;
}

export async function getCustomer(id: number): Promise<CustomerModel> {
    let result = {} as CustomerModel;
    try {
        const response = await api.get(`/Customer/${id}`);
        const { success, message, name, email, document, active } = response.data;
        if(success) {
            result = {
                id,
                name,
                document,
                email,
                active
            }
        }
    } catch (err) {

    }
    return result;
}

export async function saveCustomer({id, name, document, email, active}: CustomerModel): Promise<BaseResponse> {
    if(id > 0) {
        const response = await api.put(`/Customer/${id}`, {
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
        const response = await api.post(`/Customer`, {
            name,
            document,
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

export async function deleteCustomer(id: number): Promise<BaseResponse> {
    try {
        const response = await api.delete(`/Customer/${id}`);
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