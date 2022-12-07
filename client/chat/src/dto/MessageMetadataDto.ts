import {MetadataDto} from "./MetadataDto";

export interface MessageMetadataDto {
    content: string,
    time: string,
    metadata: MetadataDto | null,
}

