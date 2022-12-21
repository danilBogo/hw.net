import {Global, Module} from '@nestjs/common';
import {JwtModule} from "@nestjs/jwt";
import {AdminAuth} from "./adminAuth";

@Global()
@Module({
    imports: [JwtModule.register({})],
    providers: [AdminAuth],
    exports: [AdminAuth, JwtModule.register({})]
})
export class AuthModule {}
