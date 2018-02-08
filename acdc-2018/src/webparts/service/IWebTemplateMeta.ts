export interface IWebTemplateMeta {
    "$schema": string;
    actions?:   Action[];
    bindata:   Bindata;
    version:   number;
}

export interface Action {
    verb:             string;
    listName?:        string;
    templateType?:    number;
    _fillChoiceProp?: FillChoiceProp[];
    subactions?:      Subaction[];
    url?:             string;
    displayName?:     string;
    isWebRelative?:   boolean;
    themeName?:       string;
    name?:            string;
    parameters?:      Parameters;
}

export interface FillChoiceProp {
    propName:           string;
    propValue: FillChoicePropSPListTemplateType[];
}

export interface FillChoicePropSPListTemplateType {
    name: string;
    val:  number;
}

export interface Parameters {
    event:   string;
    product: string;
}

export interface Subaction {
    verb:              string;
    description?:      string;
    title?:            string;
    fieldType?:        string;
    fill?:             Fill;
    displayName?:      string;
    isRequired?:       boolean;
    addToDefaultView?: boolean;
    name?:             string;
    fieldDisplayName?: string;
    formatterJSON?:    string;
}

export interface Fill {
    propName:           string;
    SPListTemplateType: FillSPListTemplateType[];
}

export interface FillSPListTemplateType {
    name: string;
    val:  string;
}

export interface Bindata {

}
