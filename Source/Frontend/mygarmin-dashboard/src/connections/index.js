import * as React from "react";
import { List, Datagrid, TextField, BooleanField, EditButton, DateField} from 'react-admin';
import ImportDataButton from './importDataButton';

export const ConnectionList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField label="Type" source="connectionType" />
            <TextField label="Id" source="id" />
            <BooleanField label="Data Loaded" source="isDataLoaded" />
            <DateField label="Last update" source="lastUpdate" />     
            <ImportDataButton/>
            <EditButton />
        </Datagrid>
    </List>
);