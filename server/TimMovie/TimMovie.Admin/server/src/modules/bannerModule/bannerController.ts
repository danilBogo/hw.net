import {Admin} from "../authModule/adminAuth";
import {Body, Controller, Delete, Get, Param, Post, Query, UploadedFile, UseInterceptors} from "@nestjs/common";
import {BannerService} from "./bannerService";
import {BannerDto} from "../../dto/BannerDto";
import {FileInterceptor} from "@nestjs/platform-express";
import {NewBannerDto} from "../../dto/NewBannerDto";
import {Result} from "../../dto/Result";
import PaginationLoading from "../../dto/PaginationLoading";
import {UpdateBannerDto} from "../../dto/UpdateBannerDto";


@Admin()
@Controller('banners')
export class BannerController {
    constructor(private readonly bannerService: BannerService) {
    }

    @Get('getAllBanners')
    public async GetAllBanners(): Promise<BannerDto[]> {
        return await this.bannerService.GetAllBanners();
    }

    @Get('pagination')
    public async getBannersByPart(@Query() pagination: PaginationLoading): Promise<BannerDto[]> {
        return await this.bannerService.getBannersByPart(pagination)
    }

    @Post('add')
    @UseInterceptors(FileInterceptor('img'))
    async addNewBanner(@Body() newBanner: NewBannerDto, @UploadedFile() image: Express.Multer.File): Promise<Result<string>> {
        if (image == null) {
            return {
                success: false,
                textError: "Необходимо добавить обложку для баннера",
            }
        }

        return await this.bannerService.addNewBanner(newBanner, image);
    }

    @Delete('/:bannerId')
    async deleteBanner(@Param("bannerId") bannerId: string): Promise<Result<string>> {
        return await this.bannerService.deleteBanner(bannerId)
    }

    @Post('update')
    @UseInterceptors(FileInterceptor('img'))
    async updateBanner(@Body() banner: UpdateBannerDto, @UploadedFile() image: Express.Multer.File | null): Promise<Result<string>> {
        return await this.bannerService.updateBanner(banner, image)
    }
}
