declare global {
    interface Array<T> {
        lastIdeaId(): number;
    }
}

Array.prototype.lastIdeaId = function () {
    if (this.length == 0)
        return null;

    return this.slice(-1)[0].id;
};

export {};