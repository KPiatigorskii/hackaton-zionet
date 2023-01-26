import { RandomUUIDOptions } from "crypto"

export interface TeamMember{
    id: number,
    twitterAuthorId: number
}

export interface Team {
    id: number,
    name: string,
    memberArray: Array<TeamMember>
}

export interface Status {
    id : number,
    title : string,
    statusId : number
}

export interface Role {
    id : number,
    title : string,
    statusId : string,
    status : Status
}

export interface User {
    id : number,
    email : string,
    github : string,
    firstName: string,
    lastName : string,
    login : string,
    roleId : number,
    createDate : Date,
    updateDate : Date,
    createUserId : number,
    updateUserId : number,
    statusId : number,
    role: Role,
    status: Status
}

export interface TwitterRecord{
    id: number,
    authorId: number,
    participantId: number,
    eventName: string,
    teamName: string,
    enginePort: number,
    engineCronUuid: string,
    isSearching: boolean,
    alreadyFound: boolean
}