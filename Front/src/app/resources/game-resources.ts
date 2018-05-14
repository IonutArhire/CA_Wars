import { IPlayerResource } from "./player-resources";
import { IDimensionsResources } from "./dimensions-resources";

export interface IGameResources {
    dimensions: IDimensionsResources;
    nrPlayers: number;
    players: Array<IPlayerResource>
    assignedNumber: number;
}
