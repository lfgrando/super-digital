export interface LancamentoModel {
    id: string
    eOperacao : number
    eOperacaoDescricao : string
    idContaOrigem: string
    idContaDestino : string
    valor : number
    valorFormatado : string
    dataCadastroFormatada : string
}