import {Injectable, CanActivate, ExecutionContext, UseGuards, UnauthorizedException} from '@nestjs/common';
import {JwtService} from "@nestjs/jwt";
import fetch from "node-fetch"

@Injectable()
export class AdminAuth implements CanActivate {
    constructor(private jwtService: JwtService) {}

    async canActivate(context: ExecutionContext): Promise<boolean> {
        const req = context.switchToHttp().getRequest()
        try{
            const authHeaders: string | undefined = req.headers.authorization;
            if(!authHeaders){
                throw new UnauthorizedException({
                    message: 'request doesnt have any authorize headers'
                });
            }
            const [bearer, token] = authHeaders.split(' ');
            if(!((bearer === 'Bearer') && token)){
                throw new UnauthorizedException({
                    message: 'authorize header must be bearer'
                });
            }

            let authResponse = await fetch(process.env.IDENTITY_URL, {
                method: 'GET',
                headers: {
                    "Accept": "*/*",
                    "Authorization": `Bearer ${token}`
                }
            });

            if(authResponse.status != 200)
                throw new UnauthorizedException({
                    message: 'invalid token'
                })

            const decodedJwtAccessToken = this.jwtService.decode(token)
            let roles = decodedJwtAccessToken[process.env.CLAIM_ROLE];
            if(roles != null){
                if(roles.includes('admin')){
                    return true
                }
                throw new UnauthorizedException({
                    message: 'user doesnt have admin role'
                })
            }

            throw new UnauthorizedException({
                message: 'decoded jwt doesnt have role'
            })

        }
        catch (e) {
            if (e instanceof UnauthorizedException) {
                throw e;
            }
            console.error(e)
            throw new UnauthorizedException()
        }
    }
}

export const Admin = () => UseGuards(AdminAuth)