interface IPlayerResource {
    color: string;
    wins: number;
}

export interface IPlayerResources {
    allPlayersRes: Array<IPlayerResource>
    number: number;
}
