import {Injectable} from "@nestjs/common";
import {createQueryBuilder, getConnection, getRepository} from "typeorm";
import {PersonDto} from "../../dto/PersonDto";
import PaginationLoading from "../../dto/PaginationLoading";
import {Actor} from "../../../entities/Actor";
import {Producer} from "../../../entities/Producer";
import {includeNamePart} from "../../common/queryFunction";
import {Result} from "../../dto/Result";
import {UpdateBannerDto} from "../../dto/UpdateBannerDto";
import {Banner} from "../../../entities/Banner";
import {FileService} from "../FileService";

@Injectable()
export class PersonService {
    constructor(private readonly fileService: FileService) {
    }

    async getPersonsByNamePart(pagiantion: PaginationLoading): Promise<PersonDto[]> {

        let actors = (await getRepository(Actor).find({
            where: [
                {name: includeNamePart(pagiantion.namePart)},
                {surname: includeNamePart(pagiantion.namePart)},
                {
                    surname: pagiantion.namePart.split(' ').length > 1 ? includeNamePart(pagiantion.namePart.split(' ')[1]) : false,
                    name: includeNamePart(pagiantion.namePart.split(' ')[0])
                }
            ],
            order: {
                name: 'ASC',
                surname: "ASC"
            },
            take: pagiantion.take,
            skip: pagiantion.skip
        })).map(x => {
            return {
                id: x.id,
                photo: process.env.FILE_SERVICE_URL + x.photo,
                name: x.name,
                surname: x.surname,
                type: 'actor'
            }
        })

        let producers = (await getRepository(Producer).find({
            where: [
                {name: includeNamePart(pagiantion.namePart)},
                {surname: includeNamePart(pagiantion.namePart)},
                {
                    surname: pagiantion.namePart.split(' ').length > 1 ? includeNamePart(pagiantion.namePart.split(' ')[1]) : false,
                    name: includeNamePart(pagiantion.namePart.split(' ')[0])
                }
            ],
            order: {
                name: 'ASC',
                surname: "ASC"
            },
            take: pagiantion.take,
            skip: pagiantion.skip,
        })).map(x => {
            return {
                id: x.id,
                photo: process.env.FILE_SERVICE_URL + x.photo,
                name: x.name,
                surname: x.surname,
                type: 'producer'
            }
        })
        return actors.concat(producers)
    }

    async updatePersonById(id: string, person: PersonDto, image: Express.Multer.File | null): Promise<Result<string>> {
        if (person.type != 'actor' && person.type != 'producer')
            return {success: false, textError: 'Неверный тип для обновления'}
        const repository = person.type == 'actor' ? getRepository(Actor) : getRepository(Producer)
        const personToUpdate = await repository.findOne(id)
        if (personToUpdate == null) {
            return {success: false, textError: 'Не найден человек с таким id'}
        }
        if (image != null) {
            let resultSaveImage = await this.fileService.saveImage(image, 'banner');
            if (resultSaveImage.success) {
                await repository.update(id, {photo: resultSaveImage.result})
            } else {
                return {
                    success: false,
                    textError: `не удалось сохранить новое изображение: ${resultSaveImage.textError}`
                }
            }
        }
        try {
            await repository.update(id, {name: person.name, surname: person.surname})
            return {success: true}
        } catch (e) {
            return {success: false, textError: `не удалось обновить информацию о человеке: ${e.message}`}
        }
    }
}