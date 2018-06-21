import { IPlayerResource } from "./player-resources";
import { IDimensionsResources } from "./dimensions-resources";

export interface IMatchResources {
    dimensions: IDimensionsResources;
    players: Array<IPlayerResource>
    assignedNumber: number;
    map: number[][];
}
