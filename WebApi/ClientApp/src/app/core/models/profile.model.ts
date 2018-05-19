import { Tag } from "./tag.model";

export interface Profile {
    name: string;
    pictureUrl: string;
    registrationDate: string;
    sharedCount: number;
    favoritedCount: number;
    likeCount: number;
    dislikeCount: number;
    rating: number;
    contributedTags: Tag[];
}