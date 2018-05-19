export class Like {
    objectId: number;
    vote: number;

    constructor (objectId: number, vote: number) {
        this.objectId = objectId;
        this.vote = vote;
    }
}