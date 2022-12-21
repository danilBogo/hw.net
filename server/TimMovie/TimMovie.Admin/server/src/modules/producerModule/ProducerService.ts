import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";
import {PersonDto} from "../../dto/PersonDto";
import {Result} from "../../dto/Result";
import {Guid} from "guid-typescript";
import {FileService} from "../FileService";

@Injectable()
export class ProducerService{
    constructor(private readonly fileService: FileService) {
    }

    async getActorsByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }

        let producers = await getRepository(Producer)
            .find({
                where: [
                    {name: includeNamePart(namePart)},
                    {surname: includeNamePart(namePart)}
                ],
                take,
                skip,
                order: {
                    name: 'ASC',
                    surname: "ASC"
                }
            });

        let producersDto = producers.map(producer => {
            return {
                id: producer.id,
                name: `${producer.name} ${producer.surname}`
            }
        });
        return producersDto;
    }

    async getProducersByFullName(fullName: string[]): Promise<Producer[]>{
        return await getRepository(Producer)
            .find({
                where: fullName.map(value => {
                    let nameAndSurname = value.split(" ");

                    return {
                        name: nameAndSurname[0],
                        surname: nameAndSurname[1] ?? ""
                    }
                })
            });
    }

    async addProducer(newProducer: PersonDto, image: Express.Multer.File): Promise<Result<string>>{
        let resultSaveImage = await this.fileService.saveImage(image, 'producer');
        if (!resultSaveImage.success) {
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }

        const repository = getRepository(Producer)

        let producer = repository.create({
            id: Guid.create().toString(),
            name: newProducer.name,
            surname: newProducer.surname,
            photo: resultSaveImage.result
        })
        try {
            await repository.save(producer);
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }

    async deleteProducer(id: string){
        try {
            const repository = getRepository(Producer)
            await repository.delete(id)
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }
}