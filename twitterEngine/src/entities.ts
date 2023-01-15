export interface tweetEntity {
    
}

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
    Id : number,
    Title : string,
    StatusId : number
}

export interface Role {
    Id : number,
    Title : string,
    StatusId : string,
    Status : Status
}

export interface User {
        Id : number,
        Email : string,
        Github : string,
        FirstName: string,
        LastName : string,
        Login : string,
        RoleId : number,
        CreateDate : Date,
        UpdateDate : Date,
        CreateUserId : number,
        UpdateUserId : number,
        StatusId : number,
        Role: Role,
        Status: Status
    }