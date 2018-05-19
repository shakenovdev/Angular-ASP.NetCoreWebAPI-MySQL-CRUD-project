import { Creator } from "./creator.model";

export interface Comment {
    id: number;
    message: string;
    creator: Creator;
    currentUserLike: number;
    likeCount: number;
    isOP: boolean;
    createdDate: string;
    isDeleted: boolean;
}