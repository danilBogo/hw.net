import {Injectable} from "@nestjs/common";
import {getRepository, In, MoreThan, Not} from "typeorm";
import {Guid} from "guid-typescript";
import {Subscribe} from "../../../entities/Subscribe";
import {UserSubscribe} from "../../../entities/UserSubscribe";
import NameDto from "../../dto/NameDto";
import {plainToInstance} from "class-transformer";
import {addMonths} from "../../common/dateExtensions";
import SubscribeDto from "../../dto/SubscribeDto";
import {Result} from "../../dto/Result";
import {FilmService} from "../filmModule/FilmService";
import {GenreService} from "../genreModule/GenreService";
import {equalNameWithoutCase, includeNamePart} from "../../common/queryFunction";
import SubscribeInfoDto from "../../dto/SubscribeInfoDto";
import ResultActionForUser from "../../dto/ResultActionForUser";
import SubscribeAllInfoDto from "../../dto/SubscribeAllInfoDto";

@Injectable()
export class SubscribeService {
    constructor(private readonly filmService: FilmService,
                private readonly genreService: GenreService){ }
    
    public async getAllSubscribes(): Promise<NameDto[]>{
        let allSubscribes = await getRepository(Subscribe).find();
        let subscribesDto = plainToInstance(NameDto, allSubscribes);
        
        console.log(subscribesDto);
        
        return subscribesDto;
    }
    
    public async updateSubscribeForUser(userId: string, subscribeNames: string[]): Promise<void>{
        let subscribeIds = (await getRepository(Subscribe)
            .find({
                where: {
                    name: In(subscribeNames)
                }
            })).map(value => value.id);
        
        let userSubscribeRep = getRepository(UserSubscribe);
        
        let notChangedSubscribes = (await userSubscribeRep
            .find({
                where: {
                    subscribedUserId: userId,
                    subscribeId: In(subscribeIds)
                }
            })).map(value => value.subscribeId);

        let subscribeForAdd = subscribeIds.filter(id => !notChangedSubscribes.includes(id));
 
        
        await userSubscribeRep.delete({
            subscribedUserId: userId,
            subscribeId: Not(In(subscribeIds))
        });
        
        await userSubscribeRep
            .insert(subscribeForAdd.map(id => {
                return {
                    id: Guid.create().toString(),
                    subscribeId: id,
                    subscribedUserId: userId,
                    startDay: new Date(),
                    endDate: addMonths(new Date(), 1)
                }
            }));
    }
    
    public async tryAddSubscribe(subscribeDto: SubscribeDto): Promise<Result<string>>{
        if (await this.subscribeIsExisted(subscribeDto.name)){
            return {
                success: false,
                textError: "Подписка с таким именем уже существует"
            }
        }
        
        let subscribeRep = getRepository(Subscribe);
        let subscribe =  subscribeRep.create(plainToInstance(Subscribe, subscribeDto));
        subscribe.id = Guid.create().toString();
        
        if (subscribeDto.films && subscribeDto.films.length !== 0){
            let films = await this.filmService.getFilmsByNames(subscribeDto.films);
            subscribe.films = films;
        }

        if (subscribeDto.genres  && subscribeDto.genres.length !== 0){
            let genres = await this.genreService.getGenresByFullName(subscribeDto.genres);
            subscribe.genres = genres;
        }
        
        await subscribeRep.save(subscribe);
        
        return {
            success: true
        }
    }
    
    public async isExistedWithoutHimself(name: string, currentSubId: string){
        return !!(await getRepository(Subscribe)
            .findOne({
                where: {
                    name: equalNameWithoutCase(name),
                    id: Not(currentSubId)
                }
            }));
    }
    
    public async subscribeIsExisted(name: string): Promise<boolean>{
        return !!(await getRepository(Subscribe)
            .findOne({
                where: {
                    name: equalNameWithoutCase(name)
                }
            }));
    }
    
    public async getSubscribesInfoByNamePart(namePart: string, take: number, skip: number): Promise<SubscribeInfoDto[]>{
        let subscribes = await getRepository(Subscribe)
            .find({
                where: {
                    name: includeNamePart(namePart)
                },
                relations: ["films", "genres"], 
                skip: skip,
                take: take
            });
        
        let subscribesDto: SubscribeInfoDto[] = subscribes.map(subscribe => {
            let dto = plainToInstance(SubscribeInfoDto, subscribe);
            dto.films = subscribe.films.map(film => film.title);
            dto.genres = subscribe.genres.map(genre => genre.name);
            return dto;
        });
        
        return subscribesDto;
    }
    
    public async tryGetAllInfoAboutSubscribe(id: string): Promise<Result<SubscribeAllInfoDto>>{
        let subscribe = await getRepository(Subscribe)
            .findOne({
                where: {
                    id: id
                },
                relations: ["films", "genres"],
            });
        
        if (!subscribe){
            return { 
                success: false,
                textError: "Данной подписки не существует"
            };
        }

        let dto = plainToInstance(SubscribeAllInfoDto, subscribe);
        dto.films = subscribe.films.map(film => film.title);
        dto.genres = subscribe.genres.map(genre => genre.name);

        return {
            success: true,
            result: dto
        };
    }

    public async tryDeleteSubscribeById(id: string): Promise<ResultActionForUser>{
        if (await this.isActive(id)){
            return {
                success: false,
                textMessageForUser: "Ошибка. Подписка еще активна."
            }
        }
        console.log("Не активна")
        
        if (await this.hasUsers(id)){
            return {
                success: false,
                textMessageForUser: "Ошибка. Подписка еще действует для некоторых пользователей."
            }
        }
        
        let result = await getRepository(Subscribe).delete(id);
        
        if (result.affected < 1){
            return {
                success: false,
                textMessageForUser: `Произошла ошибка, попробуйте перезагрузить страницу и попробовать снова.`
            }
        }
        
        return  {
            success: true
        }
    }

    public async tryUpdateSubscribe(id: string, newSubInfo: SubscribeDto): Promise<ResultActionForUser>{
        if (await this.isExistedWithoutHimself(newSubInfo.name, id)){
            return {
                success: false,
                textMessageForUser: "Подписка с таким именем уже существует"
            }
        }
        
        let subscribe =  plainToInstance(Subscribe, newSubInfo);
        subscribe.id = id;

        if (newSubInfo.films && newSubInfo.films.length !== 0){
            let films = await this.filmService.getFilmsByNames(newSubInfo.films);
            subscribe.films = films;
        }

        if (newSubInfo.genres  && newSubInfo.genres.length !== 0){
            let genres = await this.genreService.getGenresByFullName(newSubInfo.genres);
            subscribe.genres = genres;
        }

        await getRepository(Subscribe).save(subscribe);

        return {
            success: true
        }
    }
    
    public async hasUsers(subscribeId: string){
        return !!(await getRepository(UserSubscribe)
            .findOne({
                where: {
                    endDate: MoreThan(new Date()),
                    subscribeId: subscribeId
                }
            }));
    }
    
    public async isActive(subscribeId: string): Promise<boolean>{
        return (await getRepository(Subscribe).findOne(subscribeId))
            .isActive;
    }
}
