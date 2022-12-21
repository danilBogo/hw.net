import {Module} from "@nestjs/common";
import {BannerController} from "./bannerController";
import {BannerService} from "./bannerService";
import {FileService} from "../FileService";

@Module({
    controllers: [BannerController],
    providers: [BannerService, FileService]
})
export class BannerModule {
}