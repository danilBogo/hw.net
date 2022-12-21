import {Injectable} from '@nestjs/common';
import {getRepository, Raw} from "typeorm";
import {ShortInformationAboutUserDto} from "../../dto/ShortInformationAboutUserDto";
import {AspNetUser} from "../../../entities/AspNetUser";
import {AllInformationAboutUserDto} from "../../dto/AllInformationAboutUserDto";
import {includeNamePart} from "../../common/queryFunction";

@Injectable()
export class UserService {
    public async getUsersWithFilterByLogin(incomingText: string, skip: number, take: number): Promise<ShortInformationAboutUserDto[]>{
        const userRepository = getRepository(AspNetUser);
        let users = await userRepository.find({
            where: {
                userName: includeNamePart(incomingText),
            },
            relations: ["aspNetUserClaims","userSubscribes", "userSubscribes.subscribe"],
            skip,
            take
        });
        
        let usersDto: ShortInformationAboutUserDto[] = users.map(user => {
            return {
                id: user.id,
                login: user.userName,
                email: user.email,
                roles: user.aspNetUserClaims
                    .filter(claim => claim.claimType === process.env.CLAIM_ROLE)
                    .map(claim => claim.claimValue),
                subscribes: user.userSubscribes.map(sub => sub.subscribe.name)
            }
        });
        return usersDto;
    }
    
    public async getAllInfoAboutUser(id: string): Promise<AllInformationAboutUserDto>{
        const userRepository = getRepository(AspNetUser);
        let user = await userRepository.findOne({
            where: {
                id : id,
            },
            relations: ["aspNetUserClaims","userSubscribes", "userSubscribes.subscribe", "country"],
        });
        
        if (user == null){
            return null;
        }

        let userDto: AllInformationAboutUserDto = {
            id: user.id,
            login: user.userName,
            email: user.email,
            roles: user.aspNetUserClaims
                .filter(claim => claim.claimType === process.env.CLAIM_ROLE)
                .map(claim => claim.claimValue),
            subscribes: user.userSubscribes.map(sub => sub.subscribe.name),
            displayName: user.displayName,
            registrationDate: user.registrationDate,
            birthDate: user.birthDate,
            countryName: user.country?.name ?? null,
        };
        
        return userDto; 
    }
    
    public async userIsExisted(id: string): Promise<boolean>{
        let user = await getRepository(AspNetUser)
            .findOne({where: {id: id}});
        return user != undefined;
    }
}
