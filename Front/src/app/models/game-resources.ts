import { IPlayerResource } from "./player-resources";

export interface IGameResources {
    size: number;
    nrPlayers: number;
    players: Array<IPlayerResource>
    assignedNumber: number;
}
