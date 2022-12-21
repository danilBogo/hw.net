import {Raw} from "typeorm";

export function includeNamePart(namePart: string){
    let lowerNamePart = namePart.toLowerCase();
    
    return Raw(alias => `lower(${alias}) LIKE '%${lowerNamePart}%'`);
}

export function equalNameWithoutCase(name: string){
    let lowerName = name.toLowerCase();

    return Raw(alias => `lower(${alias}) LIKE '%${lowerName}%'`);
}