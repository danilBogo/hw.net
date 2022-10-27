import {FileMetadata} from "./FileMetadataDto";

export interface MessageFileMetadataDto {
    content: string,
    time: string,
    fileMetadata: FileMetadata | null
}

