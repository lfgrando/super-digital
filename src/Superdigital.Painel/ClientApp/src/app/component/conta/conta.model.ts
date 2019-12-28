import { ClienteModel } from './cliente.model';

export interface ContaModel {
    id: string
    numero : string
    eContaTipo: number
    eContaTipoDescricao : string
    cliente : ClienteModel
    saldoFormatado : string
}
