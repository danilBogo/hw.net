import {Injectable} from "@nestjs/common";
import axios from "axios";
import {Result} from "../dto/Result";
const FormData = require('form-data');

@Injectable()
export class FileService {
    async saveFilmImage(image: Express.Multer.File): Promise<Result<string>>{
        let url = `${process.env.FILE_SERVICE_URL}/file-api/image/film`;
        const form = new FormData();
        form.append('image', image.buffer, image.originalname);
        
        let response = await axios.post(url, form);
        
        if (!response.status.toString().startsWith("2")){
            return {
                success: false
            }
        }
        
        let location = new URL(response.headers.location);
        let pathAndQuery = `${location.pathname}${location.search}`;
        console.log(pathAndQuery);
        
        return {
            result: pathAndQuery,
            success: true
        };
    }

    async saveImage(image: Express.Multer.File, entityName: string): Promise<Result<string>>{
        let url = `${process.env.FILE_SERVICE_URL}/file-api/image/${entityName}`;
        const form = new FormData();
        form.append('image', image.buffer, image.originalname);

        let response = await axios.post(url, form);

        if (!response.status.toString().startsWith("2")){
            return {
                success: false
            }
        }

        let location = new URL(response.headers.location);
        let pathAndQuery = `${location.pathname}${location.search}`;
        console.log(pathAndQuery);

        return {
            result: pathAndQuery,
            success: true
        };
    }

}