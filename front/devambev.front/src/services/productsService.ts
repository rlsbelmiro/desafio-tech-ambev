import { setupAPIClient } from "./app";


export const api = setupAPIClient();

export interface ProductModel {
    id: number,
    name: string,
    description: string,
    active: boolean,
    quantity: number,
    price: number,
    message?: string,
    success?: boolean
}

export interface BaseResponse {
    message: string,
    success: boolean
}

export async function listProducts():  Promise<ProductModel[]> {
    let result = [] as ProductModel[];
    try {
        const response = await api.get('/Product');
        const { success, message, products } = response.data;
        if(success) {
            result = products;
        }
    } catch (err) {

    }
    return result;
}

export async function getProduct(id: number): Promise<ProductModel> {
    let result = {} as ProductModel;
    try {
        const response = await api.get(`/Product/${id}`);
        const { success, message, name, description, price, quantity, active } = response.data;
        if(success) {
            result = {
                id,
                name,
                description,
                price,
                quantity,
                active,
                message,
                success
            }
        }
    } catch (err) {

    }
    return result;
}

export async function saveProduct({id, name, price, description, active, quantity}: ProductModel): Promise<BaseResponse> {
    if(id > 0) {
        const response = await api.put(`/Product/${id}`, {
            name,
            price,
            description,
            active,
            quantity
        });
        const { success, message } = response.data;
        return {
            success,
            message
        };
    } else {
        const response = await api.post(`/Product`, {
            name,
            price,
            description,
            active,
            quantity
        });
        const { success, message } = response.data;
        return {
            success,
            message
        };
    }
}

export async function deleteProduct(id: number): Promise<BaseResponse> {
    try {
        const response = await api.delete(`/Product/${id}`);
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