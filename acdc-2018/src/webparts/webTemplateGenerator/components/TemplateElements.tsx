import * as React from "react";
import styles from "./WebTemplateGenerator.module.scss";
import { findIndex } from "@microsoft/sp-lodash-subset";
import WebTemplateMeta from "../../../../lib/webparts/service/WebTemplateMeta";
import RenderWebAction from "./RenderWebAction";
import { IWebTemplateGeneratorProps } from "./IWebTemplateGeneratorProps";
import { IWebTemplateMeta, Action } from "../../service/IWebTemplateMeta";
let cloneDeep = require("clone-deep");
// import {  } from '@microsoft/sp-lodash-subset';

export interface ITemplateElementsProps {

}
export interface ITemplateElementsState {
    elements: IWebTemplateMeta;
}

export default class TemplateElements extends React.Component<ITemplateElementsProps, ITemplateElementsState> {
    constructor(props: ITemplateElementsProps) {
        super(props);

        this.state = {
            elements: {
                "$schema": "schema.json",
                "actions": [],
                "bindata": { },
                "version": 1
            }
        };
    }

    public render(): React.ReactElement<ITemplateElementsProps> {
      return (
        <div className={styles.webTemplateGenerator}>
            <select style={{width: 200}} onChange={(val) => {
                let selectedAction: Action = WebTemplateMeta.actions[findIndex(
                    WebTemplateMeta.actions, (curAction, i) => curAction.verb === val.target.value
                )];

                if (!selectedAction) { console.error("selectedAction not found@"); return; }

                let elements: IWebTemplateMeta = cloneDeep(this.state.elements);
                elements.actions.push(selectedAction);
                this.setState({ elements: elements });
            }}>
                <option title="" value="None">Choose Web Template Action</option>
                {WebTemplateMeta.actions.map(action => {
                    return (
                        <option title={action.verb} value={action.verb} key={action.verb + Math.random().toString()}>
                            {action.verb}
                        </option>
                    );
                })}
            </select>
            {this.state.elements.actions.map(action => {
                return <div key={action.verb + Math.random().toString()}><RenderWebAction action={action}/></div>;
            })}
        </div>
      );
    }
  }
