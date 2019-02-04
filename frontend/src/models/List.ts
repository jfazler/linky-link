import ILink from "./ILink";
import Array from "../shared/Array";

export default class List {
  editable: boolean = false;

  constructor(
    public vanityUrl: string = "",
    public description: string = "",
    public links: Array<ILink> = new Array(),
    public isNew: boolean = false
  ) {}
}
