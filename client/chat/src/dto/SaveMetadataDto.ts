import {MetadataDto} from "./MetadataDto";

export interface SaveMetadataDto {
    requestId: string,
    metadata: MetadataDto,
    userId: string
}

