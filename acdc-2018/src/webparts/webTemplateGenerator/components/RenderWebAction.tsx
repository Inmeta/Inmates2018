import * as React from 'react';
import styles from './WebTemplateGenerator.module.scss';
import { IWebTemplateGeneratorProps } from './IWebTemplateGeneratorProps';
import { Action } from '../../service/IWebTemplateMeta';
import WebTemplateMeta, { RenderAction } from '../../service/WebTemplateMeta';

// import {  } from '@microsoft/sp-lodash-subset';

export interface IRenderWebActionProps {
    action: Action;
}
export interface IRenderWebActionState {
    action: Action;
}

export default class RenderWebAction extends React.Component<IRenderWebActionProps, IRenderWebActionState> {
    constructor(props: IRenderWebActionProps) {
        super(props);

        this.state = { "action": this.props.action };
    }

    public render(): React.ReactElement<IRenderWebActionProps> {
      return (
        <div className={"ms-Grid-row"}>
            {RenderAction(this.state.action)}
        </div>
      );
    }
  }
