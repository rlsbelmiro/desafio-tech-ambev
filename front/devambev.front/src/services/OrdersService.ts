import { setupAPIClient } from "./app";

export const api = setupAPIClient();

export interface OrderModel {
    id: number,
    customerId: number,
    amount?: number,
    customerName?: string,
    items: OrderItemModel[]
}

export interface OrderItemModel {
    quantity: number;
    productId: number;
    productName?: string;
    price?: string;
}

export interface BaseResponse {
    message: string,
    success: boolean
}

export async function listOrders():  Promise<OrderModel[]> {
    let result = [] as OrderModel[];
    try {
        const response = await api.get('/Order');
        const { success, message, orders } = response.data;
        if(success) {
            result = orders;
        }
    } catch (err) {

    }
    return result;
}

export async function getOrder(id: number): Promise<OrderModel> {
    let result = {} as OrderModel;
    try {
        const response = await api.get(`/Order/${id}`);
        const { success, message, customerId, items } = response.data;
        if(success) {
            result = {
                id,
                customerId,
                items
            }
        }
    } catch (err) {

    }
    return result;
}

export async function saveOrder({customerId, items}: OrderModel): Promise<BaseResponse> {
    const response = await api.post(`/Order`, {
        customerId,
        items
    });
    const { success, message } = response.data;
    return {
        success,
        message
    };
}