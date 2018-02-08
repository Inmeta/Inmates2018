import * as React from 'react';
import styles from './WebTemplateGenerator.module.scss';
import { IWebTemplateGeneratorProps } from './IWebTemplateGeneratorProps';
import { escape } from '@microsoft/sp-lodash-subset';
import TemplateElements from './TemplateElements';

export default class WebTemplateGenerator extends React.Component<IWebTemplateGeneratorProps, {}> {
  public render(): React.ReactElement<IWebTemplateGeneratorProps> {
    return (
      <div className={styles.webTemplateGenerator}>
        <div className={styles.container}>
          <div className={`ms-Grid-row ms-bgColor-themeDark ms-fontColor-white ${styles.row}`}>
            <div className="ms-Grid-col ms-lg12 ms-xl12 ">
              <span className="ms-font-xl ms-fontColor-white">Site Template Generator</span>
              <p className="ms-font-l ms-fontColor-white">Customize SharePoint experiences using Web Parts.</p>

              <TemplateElements />

              <p className="ms-font-l ms-fontColor-white">{escape(this.props.description)}</p>
              <a href="https://aka.ms/spfx" className={styles.button}>
                <span className={styles.label}>Save</span>
              </a>
            </div>
          </div>
        </div>
      </div>
    );
  }
}
