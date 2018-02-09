import * as React from "react";
import styles from './../webTemplateGenerator/components/WebTemplateGenerator.module.scss';
import { Action, IWebTemplateMeta } from "./IWebTemplateMeta";
import { findIndex } from "@microsoft/sp-lodash-subset";
export const RenderAction = (action: Action) => {
    let actionElement: JSX.Element = <div>Not implemented</div>;
    let actionElements: JSX.Element[] = [];
    if (action._fillChoiceProp && action._fillChoiceProp.length) {

        actionElements = Object.keys(action)
        .filter(actionProp => ["verb", "_fillChoiceProp", "subactions"].indexOf(actionProp) == -1)
        .map(actionProp => {
            if (action._fillChoiceProp[0].propName === actionProp) {
                return (
                    <select style={{width: 200}}>
                        <option title="" value="None">{action.name}</option>
                        {action._fillChoiceProp[0].propValue.map(choiceValue => {
                            return (
                                <option title={choiceValue.name} value={choiceValue.val} key={Math.random()}>
                                    {choiceValue.name}
                                </option>
                            );
                        })}
                    </select>
                );
            } else if (typeof action[actionProp] === "string") {
                return <input type="text" value={actionProp} />;
            } else if (typeof action[actionProp] === "number") {
                return <input type="number"value={actionProp}  />;
            }
        });
        // actionElement = (
        //     <select>
        //         <option title="" value="None">Text</option>
        //         {action._fillChoiceProp.map(action => {
        //             return (
        //                 <option title={action.verb} value={action.verb} key={action.verb}>
        //                     {action.verb}
        //                 </option>
        //             );
        //         })}
        //     </select>

        // );

        return (
            <div  className="ms-Class-row"  >
                {actionElements.map(el => { return <div className="ms-Class-row" key={Math.random()}>{el}</div>; })}
            </div>
        );
    }


}

export default {
    "$schema": "schema.json",
        "actions": [
            {
                "verb": "createSPList",
                "listName": "Customer Tracking",
                "templateType": 100,
                "_fillChoiceProp": [
                    {
                        "propName": "templateType",
                        "propValue": [
                            { "name": "Announcements", "val": 100 },
                            { "name": "Document library", "val": 101 },
                            { "name": "Survey", "val": 102 },
                            { "name": "Links", "val": 103 },
                            { "name": "Announcements", "val": 104 },
                            { "name": "Contacts", "val": 105 },
                            { "name": "Calendar", "val": 106 },
                            { "name": "Tasks", "val": 107 },
                            { "name": "Discussion board", "val": 108 },
                            { "name": "Picture library", "val": 10 }
                        ]
                    }
                ],
                "subactions": [
                    {
                        "verb": "setDescription",
                        "description": "List of Customers and Orders"
                    },
                    {
                        "verb": "setTitle",
                        "title": "Customers and Orders"
                    },
                    {
                        "verb": "addSPField",
                        "fieldType": "Text",
                        "fill": {
                            "propName": "fieldType",
                            "SPListTemplateType": [
                                { "name": "Text", "val": "Text" },
                                { "name": "Note", "val": "Note" },
                                { "name": "Number", "val": "Number" },
                                { "name": "Boolean", "val": "Boolean" },
                                { "name": "User", "val": "User" },
                                { "name": "DateTime", "val": "DateTime" }
                            ]
                        },
                        "displayName": "Customer Name",
                        "isRequired": false,
                        "addToDefaultView": true
                    },
                    {
                        "verb": "deleteSPField",
                        "displayName": "Modified"
                    },
                    {
                        "verb": "addContentType",
                        "name": "name"
                    },
                    {
                        "verb": "removeContentType",
                        "name": "name"
                    },
                    {
                        "verb": "setSPFieldCustomFormatter",
                        "fieldDisplayName": "name",
                        "formatterJSON": "formatterJSON"
                    }
                ]
            },
            {
                "verb": "addNavLink",
                "url": "/Customer Event Collateral",
                "displayName": "Event Collateral",
                "isWebRelative": true
            },
            {
                "verb": "applyTheme",
                "themeName": "Blue Yonder"
            },
            {
                "verb": "setSiteLogo",
                "url": "/Customer Event Collateral/logo.jpg"
            },
            {
                "verb": "triggerFlow",
                "url": "<A trigger URL of the Flow.>",
                "name": "Record and tweet site creation event",
                "parameters": {
                    "event": "Microsoft Event",
                    "product": "SharePoint"
                }
            }
        ],
        "bindata": { },
        "version": 1
    } as IWebTemplateMeta;