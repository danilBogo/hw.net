import {Injectable} from '@nestjs/common';
import {getRepository} from "typeorm";
import {AspNetUserClaim} from "../../../entities/AspNetUserClaim";
import NameDto from "../../dto/NameDto";
import {RoleNames} from "../../consts/RoleNames";
import {Guid} from "guid-typescript";

@Injectable()
export class RoleService {
   public getAllRoles(): NameDto[] {
       let names: NameDto[] = [...RoleNames].map(role => {
           return {
               id: Guid.create().toString(),
               name: role
           }
       });
       
       return names;
   }

    public async updateRolesForUser(userId: string, roleNames: string[]): Promise<void>{
       let userClaimRep = getRepository(AspNetUserClaim);
        await userClaimRep
            .delete({
                claimType: process.env.CLAIM_ROLE,
                userId: userId
            });
        let checkedRoles = roleNames.filter(value => RoleNames.has(value));

        await userClaimRep
            .insert(checkedRoles.map(roleName => {
                return {
                    claimType: process.env.CLAIM_ROLE,
                    claimValue: roleName,
                    userId: userId
                }
            }));
    }
}
