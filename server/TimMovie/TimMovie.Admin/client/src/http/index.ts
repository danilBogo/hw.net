import axios, {AxiosRequestConfig} from "axios";

const $api = axios.create({
    withCredentials: true,
    baseURL: process.env.REACT_APP_SERVER_URL
})

$api.interceptors.request.use( (config: AxiosRequestConfig) => {
    config.headers = {
        'Authorization' : `Bearer ${localStorage.getItem(`${process.env.REACT_APP_ACCESS_TOKEN_KEY_NAME}`)}`,
        'Access-Control-Allow-Origin' : '*',
    }
    return config
})

export default $api;