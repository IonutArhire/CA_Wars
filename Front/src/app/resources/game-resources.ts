import { IPlayerResource } from "./player-resources";
import { IDimensionsResources } from "./dimensions-resources";

export interface IGameResources {
    dimensions: IDimensionsResources;
    players: Array<IPlayerResource>
    assignedNumber: number;
    map: number[][];
}
