# rabbitmq-net
Repositório para praticar conhecimentos acerca do message broker RabbitMQ.

## Resumo sobre os papéis na mensageria utilizando RabbitMQ

- **Ator**: qualquer parte do código, pode ser um serviço, um sistema inteiro, alguém que realiza uma operação.

- **Producer**: um producer publisher é um ator que publica uma mensagem no *message broker*. Embora possa se ter o ímpetto de pensar: "é quem publica uma mensagem na fila", na prática vamos ver que há um intermediador, que é a *exchage*, sendo o seu papel no fluxo de validar essa informação. Portanto: alguém que está tentando publicar uma mensagem no message broker. 

- **Consumer**: é um ator que consume mensagens de uma ou mais filas. Representa quem realiza o processamento de uma mensagem que está na fila. 

- **Virtual host**: ambiente isolado dentro de uma instância do RabbitMQ. Nele temos usuários e funções, e todos os objetos como: filas, exchanges, e suas ligações ou binds. Cada *virtual host* é isolado dos demais. 

- **Mensagem**: objeto que tem o propósito de chegar a uma ou mais filas para assim ser processada (s). Uma mensagem AMQP consiste em um envelope, nesse envelope temos *headers* (semelhantes aos headers http) e *payload* (o corpo efetivo da mensagem). 

    - Os cabeçalhos são úteis para diversas finalidades, inclusive de **roteamento**. Alguns tipos de *exchange* usam esses cabeçalhos em conjunto com os *binds* para decidir em qual ou quais filas uma mensagem deve ser enviada. Os cabeçalhos também são muito úteis e podem te ajudar a entender o tipo da mensagem (formato de *encoding*, etc). 

- **Exchange**: é um objeto programável de **roteamento**. Isso quer dizer, que pode definir um conjunto de regras de **roteamento**. Essas regras podem fazer uma mensagem ir direto para uma fila, ou mesmo ser distribuída em diversas filas ou em outros casos ser descartada. Um *publisher*, sempre envia mensagem para uma *exchange*. **No RabbitMQ, não existe envio de mensagem direto para filas.**

### Tipos de exchanges:
    - Direct: exerce o papel de uma exchange fake, pois ela não toma decisão alguma, ela não possui nome, faz um bypass para enviar a mensagem para a fila que possui o nome que foi definida. 
    - Fanout
    - Topic
    - Headers

- **Filas**: objetos internos do RabbitMQ que armazenam mensagens e gerenciam sua distribuição para os consumidores. É a partir delas que as mensagens são consumidas. 
    - Um tipo de fila muito comum, são filas dinâmicas, criadas por um consumidor. 

- **Filas nomeadas:** possuem um nome pré-definido. São filas que possuem "CPF", o nome dela tem relevância para sua implantação.
- **Filas anônimas**: são filas criadas dinamicamente por demanda. Ao criar uma fila anônima, seu provider *AMQP* recebe no código o nome da fila, criado dinamicamente pelo *RabbitMQ*. 
- **Durável ou não durável:** A fila persiste entre *restarts* do RabbitMQ?
- **Exclusiva ou não exclusiva:** somente 1 consumidos poderá consumir a fila, e quando o consumidor cair a fila será deletada. 

# Exemplo 1
 
Retirado da [documentação](https://www.rabbitmq.com/tutorials/tutorial-one-dotnet.html) exemplo contendo um produtor que envia uma única mensagem e um consumidor que recebe as mensagens e as imprime.

![figura-representativa](https://www.rabbitmq.com/img/tutorials/python-one.png)

