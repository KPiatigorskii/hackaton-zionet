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