export class MatchCreate {
    private nrPlayers: number;
    private ruleset: string;
    private maxIters: number;
    private rows: number;
    private cols: number;

    public constructor(nrPlayers: number, ruleset: string, maxIters: number, rows: number, cols: number) {
        this.nrPlayers = nrPlayers;
        this.ruleset = ruleset;
        this.maxIters = maxIters;
        this.rows = rows;
        this.cols = cols;
    }
}