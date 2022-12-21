import {Injectable} from "@nestjs/common";
import {getRepository} from "typeorm";
import {Actor} from "../../../entities/Actor";
import {includeNamePart} from "../../common/queryFunction";
import NameDto from "../../dto/NameDto";
import {PersonDto} from "../../dto/PersonDto";
import {Result} from "../../dto/Result";
import {Guid} from "guid-typescript";
import {FileService} from "../FileService";

@Injectable()
export class ActorService {
    constructor(private readonly fileService: FileService) {
    }

    async getActorsByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]> {
        if (namePart == null) {
            return [];
        }

        let actors = await getRepository(Actor)
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

        let actorsDto = actors.map(actor => {
            return {
                id: actor.id,
                name: `${actor.name} ${actor.surname}`
            }
        });
        return actorsDto;
    }

    async getActorsByFullName(fullName: string[]): Promise<Actor[]> {
        return await getRepository(Actor)
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

    async addActor(newActor: PersonDto, image: Express.Multer.File): Promise<Result<string>> {
        let resultSaveImage = await this.fileService.saveImage(image, 'actor');
        if (!resultSaveImage.success) {
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }

        const repository = getRepository(Actor)

        let actor = repository.create({
            id: Guid.create().toString(),
            name: newActor.name,
            surname: newActor.surname,
            photo: resultSaveImage.result
        })
        try {
            await repository.save(actor);
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }

    async deleteActor(id: string){
        try {
            const repository = getRepository(Actor)
            await repository.delete(id)
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }
}