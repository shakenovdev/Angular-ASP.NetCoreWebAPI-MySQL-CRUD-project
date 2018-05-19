import { Tag } from "./tag.model";
import { Creator } from "./creator.model";

export interface Idea {
    id: number;
    title: string;
    article: string;
    tags: Tag[];
    creator: Creator;
    currentUserLike: number;
    currentUserIsFavorited: boolean;
    viewCount: number;
    likeCount: number;
    commentCount: number;
    createdDate: string;
    isDeleted: boolean;
}
