import * as React from "react";
import { Action, IWebTemplateMeta } from "./IWebTemplateMeta";
import { findIndex } from "@microsoft/sp-lodash-subset";
export const RenderAction = (action: Action) => {
    let actionElement: JSX.Element = <div>Not implemented</div>;
    let actionElements: JSX.Element[] = [];
    if (action._fillChoiceProp && action._fillChoiceProp.length) {
        Object.keys(action)
        .filter(actionProp => !["verb", "_fillChoiceProp", "subactions"].indexOf(actionProp))
        .map(actionProp => {
            if (typeof action._fillChoiceProp[0].propName === actionProp) {
                actionElements.push(<div key={Math.random().toString()}>Select one of values</div>)
            } else if (typeof action[actionProp] === "string") {
                actionElements.push(<input type="text" key={Math.random().toString()} value={actionProp} />);
            } else if (typeof action[actionProp] === "number") {
                actionElements.push(<input type="number" key={Math.random().toString()} value={actionProp}  />);
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

        return <div>{actionElements.map(el => { return el; })} </div>;
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